namespace ONE_Classics_Nexus.WinForms.Forms.Appraisals
{
    public sealed class AppraisalWorkspaceShellStatusChangedEventArgs : EventArgs
    {
        public AppraisalWorkspaceShellStatusChangedEventArgs(
            string statusText,
            string contextText,
            bool isBusy,
            bool hasUnsavedChanges,
            bool hasError,
            string? displayNumber,
            AppraisalWorkspaceSection activeSection)
        {
            StatusText = statusText;
            ContextText = contextText;
            IsBusy = isBusy;
            HasUnsavedChanges = hasUnsavedChanges;
            HasError = hasError;
            DisplayNumber = displayNumber;
            ActiveSection = activeSection;
        }

        public string StatusText { get; }

        public string ContextText { get; }

        public bool IsBusy { get; }

        public bool HasUnsavedChanges { get; }

        public bool HasError { get; }

        public string? DisplayNumber { get; }

        public AppraisalWorkspaceSection ActiveSection { get; }
    }
}