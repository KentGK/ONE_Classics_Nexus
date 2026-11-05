using ONE_Classics_Nexus.WinForms.Services.Api;
namespace ONE_Classics_Nexus.WinForms.Forms.AppraisalRequests;
public sealed class AppraisalRequestListForm : Form
{
    private readonly AppraisalRequestApiClient _api; private readonly DataGridView _grid=new(){Dock=DockStyle.Fill,ReadOnly=true,AutoGenerateColumns=true};
    private readonly ComboBox _filter=new(){DropDownStyle=ComboBoxStyle.DropDownList,Width=180};
    public AppraisalRequestListForm(AppraisalRequestApiClient api){_api=api;Text="Appraisal Requests";BackColor=Color.White;var bar=new FlowLayoutPanel{Dock=DockStyle.Top,Height=44};_filter.Items.AddRange(["Open","NeedsInformation","Qualified","Accepted","Converted","Declined","All"]);_filter.SelectedIndex=0;var refresh=new Button{Text="Refresh"};refresh.Click+=async(_,__)=>await LoadRows();bar.Controls.AddRange([_filter,refresh]);Controls.Add(_grid);Controls.Add(bar);Shown+=async(_,__)=>await LoadRows();}
    private async Task LoadRows(){var selected=_filter.SelectedItem?.ToString();var status=selected switch{"Open"=>null,"All"=>null,_=>selected};var r=await _api.SearchAsync(status);_grid.DataSource=r?.Items.ToList();}
}
