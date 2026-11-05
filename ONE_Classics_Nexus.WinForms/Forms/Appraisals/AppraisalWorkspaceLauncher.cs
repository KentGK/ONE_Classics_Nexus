using Microsoft.Extensions.DependencyInjection;

namespace ONE_Classics_Nexus.WinForms.Forms.Appraisals;

public static class AppraisalWorkspaceLauncher
{
    public static async Task<AppraisalWorkspaceForm> OpenInAsync(
        this Control host,
        IServiceProvider services,
        Guid appraisalId,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(host);
        ArgumentNullException.ThrowIfNull(services);

        if (appraisalId == Guid.Empty)
            throw new ArgumentException("Appraisal id is required.", nameof(appraisalId));

        var workspace = Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<AppraisalWorkspaceForm>(services);
        workspace.TopLevel = false;
        workspace.FormBorderStyle = FormBorderStyle.None;
        workspace.Dock = DockStyle.Fill;

        host.SuspendLayout();
        try
        {
            foreach (Control existing in host.Controls.Cast<Control>().ToArray())
                existing.Dispose();

            host.Controls.Clear();
            host.Controls.Add(workspace);
            workspace.Show();
        }
        finally
        {
            host.ResumeLayout(true);
        }

        await workspace.LoadAppraisalAsync(appraisalId, cancellationToken);
        return workspace;
    }
}
