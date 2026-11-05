using ONE_Classics_Nexus.Application.AppraisalRequests;
using ONE_Classics_Nexus.WinForms.Services.Api;
namespace ONE_Classics_Nexus.WinForms.Forms.AppraisalRequests;
public sealed class AppraisalRequestForm : Form
{
    private readonly AppraisalRequestApiClient _api; private AppraisalRequestResponse? _model;
    private readonly Label _header=new(){Dock=DockStyle.Top,Height=38,Font=new Font("Segoe UI",14,FontStyle.Bold)};
    private readonly TextBox _purpose=new(){Width=420}; private readonly TextBox _subject=new(){Width=420}; private readonly FlowLayoutPanel _actions=new(){Dock=DockStyle.Bottom,Height=46};
    public AppraisalRequestForm(AppraisalRequestApiClient api){_api=api;Text="Appraisal Request";BackColor=Color.White;var body=new FlowLayoutPanel{Dock=DockStyle.Fill,FlowDirection=FlowDirection.TopDown,Padding=new Padding(16),AutoScroll=true};body.Controls.AddRange([new Label{Text="Requested Purpose"},_purpose,new Label{Text="Subject Description"},_subject]);foreach(var a in new[]{"submit","begin-review","needs-information","qualify","accept","decline","cancel","withdraw"}){var b=new Button{Text=a,Tag=a,AutoSize=true};b.Click+=Action;b.BackColor=Color.FromArgb(14,40,65);b.ForeColor=Color.White;_actions.Controls.Add(b);}Controls.Add(body);Controls.Add(_actions);Controls.Add(_header);}
    public async Task LoadAsync(Guid id){_model=await _api.GetAsync(id);if(_model is null)return;_header.Text=$"{_model.RequestNumber}   {_model.RequestStatus} / {_model.QualificationStatus}";_purpose.Text=_model.RequestedPurpose;_subject.Text=_model.SubjectDescription??$"{_model.SubjectYear} {_model.SubjectMake} {_model.SubjectModel}";}
    private async void Action(object? sender,EventArgs e){if(_model is null||sender is not Button b)return;var action=(string)b.Tag!;object body=action is "decline" or "cancel" or "withdraw"?new ReasonRequest("Entered from Appraisal Request workspace",_model.RowVersion):action=="needs-information"?new NeedsInformationRequest("Additional information requested",_model.RowVersion):action=="qualify"?new QualifyRequest(null,_model.RowVersion):new RowVersionRequest(_model.RowVersion);_model=await _api.WorkflowAsync(_model.AppraisalRequestId,action,body);if(_model is not null)_header.Text=$"{_model.RequestNumber}   {_model.RequestStatus} / {_model.QualificationStatus}";}
}
