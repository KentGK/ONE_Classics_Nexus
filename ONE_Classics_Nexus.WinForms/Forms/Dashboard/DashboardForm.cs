using ONE_Classics_Nexus.WinForms.Models.Ui;
using ONE_Classics_Nexus.WinForms.Services.Api;

namespace ONE_Classics_Nexus.WinForms.Forms.Dashboard
{
    public partial class DashboardForm : Form
    {
        private readonly DashboardApiClient _dashboardApiClient;
        private CancellationTokenSource? _loadCts;
        private bool _hasLoaded;

        public event EventHandler? NewAppraisalRequested;
        public event EventHandler? AppraisalRequestWorkspaceRequested;
        public event EventHandler<Guid>? AppraisalRequested;

        public DashboardForm()
        {
            InitializeComponent();

            var httpClient = BuildHttpClient();
            _dashboardApiClient = new DashboardApiClient(httpClient);

            ConfigureListViews();
        }

        private static HttpClient BuildHttpClient()
        {
            return new HttpClient
            {
                BaseAddress = new Uri("https://localhost:5001")
            };
        }

        private void ConfigureListViews()
        {
            lvRecentActivity.FullRowSelect = true;
            lvRecentActivity.MultiSelect = false;
            lvRecentActivity.HideSelection = false;

            lvWorkQueue.FullRowSelect = true;
            lvWorkQueue.MultiSelect = false;
            lvWorkQueue.HideSelection = false;
            lvWorkQueue.ItemActivate += lvWorkQueue_ItemActivate;
            lvWorkQueue.MouseClick += lvWorkQueue_MouseClick;
        }

        protected override async void OnShown(EventArgs e)
        {
            base.OnShown(e);

            if (_hasLoaded)
                return;

            _hasLoaded = true;
            await LoadDashboardAsync();
        }

        private async Task LoadDashboardAsync()
        {
            CancelPendingLoad();
            _loadCts = new CancellationTokenSource();

            try
            {
                SetBusyState(true);
                SetStatus("Loading dashboard...");

                var dto = await _dashboardApiClient.GetDashboardAsync(_loadCts.Token);

                BindSummary(dto.Summary);
                BindRecentActivity(dto.RecentActivity);
                BindWorkQueue(dto.WorkQueue);

                SetStatus($"Dashboard loaded at {DateTime.Now:t}");
            }
            catch (OperationCanceledException)
            {
                SetStatus("Dashboard load canceled.");
            }
            catch (Exception ex)
            {
                SetStatus("Dashboard load failed.");
                ShowLoadError(ex);
                BindEmptyState();
            }
            finally
            {
                SetBusyState(false);
            }
        }

        private void BindSummary(DashboardSummaryDto summary)
        {
            lblActiveAppraisalsValue.Text = summary.ActiveAppraisals.ToString();
            lblPendingInspectionsValue.Text = summary.PendingInspections.ToString();
            lblValuationsReadyValue.Text = summary.ValuationsReady.ToString();
            lblNeedsReviewValue.Text = summary.NeedsReview.ToString();
        }

        private void BindRecentActivity(System.Collections.Generic.IEnumerable<DashboardActivityDto> items)
        {
            lvRecentActivity.BeginUpdate();

            try
            {
                lvRecentActivity.Items.Clear();

                foreach (var item in items)
                {
                    var localTime = item.OccurredAtUtc.ToLocalTime();

                    var row = new ListViewItem(localTime.ToString("g"));
                    row.SubItems.Add(item.ActivityType);
                    row.SubItems.Add(item.VehicleDisplay);
                    row.SubItems.Add(item.Status);

                    ApplyStatusColor(row, item.Status);

                    lvRecentActivity.Items.Add(row);
                }
            }
            finally
            {
                lvRecentActivity.EndUpdate();
            }
        }

        private void BindWorkQueue(System.Collections.Generic.IEnumerable<DashboardWorkQueueItemDto> items)
        {
            lvWorkQueue.BeginUpdate();

            try
            {
                lvWorkQueue.Items.Clear();

                foreach (var item in items)
                {
                    var row = new ListViewItem(item.AppraisalNumber)
                    {
                        Tag = item.AppraisalId,
                        ForeColor = Color.FromArgb(31, 78, 121),
                        Font = new Font(lvWorkQueue.Font, FontStyle.Underline)
                    };
                    row.SubItems.Add(item.VehicleDisplay);
                    row.SubItems.Add(item.Stage);
                    row.SubItems.Add(item.Priority);

                    ApplyPriorityColor(row, item.Priority);

                    lvWorkQueue.Items.Add(row);
                }
            }
            finally
            {
                lvWorkQueue.EndUpdate();
            }
        }

        private void lvWorkQueue_ItemActivate(object? sender, EventArgs e)
        {
            OpenSelectedAppraisal();
        }

        private void lvWorkQueue_MouseClick(object? sender, MouseEventArgs e)
        {
            if (lvWorkQueue.HitTest(e.Location).Item is not null)
                OpenSelectedAppraisal();
        }

        private void OpenSelectedAppraisal()
        {
            if (lvWorkQueue.SelectedItems.Count > 0 &&
                lvWorkQueue.SelectedItems[0].Tag is Guid appraisalId &&
                appraisalId != Guid.Empty)
            {
                AppraisalRequested?.Invoke(this, appraisalId);
            }
        }

        private void BindEmptyState()
        {
            lblActiveAppraisalsValue.Text = "0";
            lblPendingInspectionsValue.Text = "0";
            lblValuationsReadyValue.Text = "0";
            lblNeedsReviewValue.Text = "0";

            lvRecentActivity.Items.Clear();
            lvWorkQueue.Items.Clear();
        }

        private void ApplyStatusColor(ListViewItem row, string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return;

            switch (status.Trim().ToLowerInvariant())
            {
                case "ready":
                case "complete":
                case "completed":
                    row.ForeColor = Color.DarkGreen;
                    break;

                case "in progress":
                case "open":
                case "updated":
                    row.ForeColor = Color.FromArgb(31, 78, 121);
                    break;

                case "needs review":
                case "review":
                    row.ForeColor = Color.Firebrick;
                    break;
            }
        }

        private void ApplyPriorityColor(ListViewItem row, string priority)
        {
            if (string.IsNullOrWhiteSpace(priority))
                return;

            switch (priority.Trim().ToLowerInvariant())
            {
                case "high":
                    row.ForeColor = Color.Firebrick;
                    break;

                case "medium":
                    row.ForeColor = Color.DarkGoldenrod;
                    break;

                case "low":
                    row.ForeColor = Color.FromArgb(31, 78, 121);
                    break;
            }
        }

        private void SetBusyState(bool isBusy)
        {
            btnRefresh.Enabled = !isBusy;
            btnNewAppraisal.Enabled = !isBusy;
            btnAppraisalRequests.Enabled = !isBusy;
            btnNewInspection.Enabled = !isBusy;
            btnRunValuation.Enabled = !isBusy;

            progressBar.Style = isBusy ? ProgressBarStyle.Marquee : ProgressBarStyle.Blocks;
            progressBar.Visible = isBusy;
        }

        private void SetStatus(string message)
        {
            lblStatus.Text = message;
        }

        private void ShowLoadError(Exception ex)
        {
            MessageBox.Show(
                $"Unable to load dashboard data.{Environment.NewLine}{Environment.NewLine}{ex.Message}",
                "Dashboard Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private void CancelPendingLoad()
        {
            if (_loadCts == null)
                return;

            if (!_loadCts.IsCancellationRequested)
                _loadCts.Cancel();

            _loadCts.Dispose();
            _loadCts = null;
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadDashboardAsync();
        }

        private void btnNewAppraisal_Click(object sender, EventArgs e)
        {
            NewAppraisalRequested?.Invoke(this, EventArgs.Empty);
        }

        private void btnNewInspection_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Wire this to open the New Inspection workflow.",
                "New Inspection",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnAppraisalRequests_Click(object sender, EventArgs e)
        {
            AppraisalRequestWorkspaceRequested?.Invoke(this, EventArgs.Empty);
        }

        private void btnRunValuation_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Wire this to open the Valuation workflow.",
                "Run Valuation",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            CancelPendingLoad();
            base.OnFormClosing(e);
        }

        private void lblSubTitle_Click(object sender, EventArgs e)
        {

        }
    }
}