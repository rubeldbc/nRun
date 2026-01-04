using nRun.Models;

namespace nRun.UI.Forms;

public partial class TikTokScheduleForm : Form
{
    private TkSchedule? _schedule;

    public TkSchedule? ResultSchedule => _schedule;

    public TikTokScheduleForm(TkSchedule? existingSchedule = null)
    {
        InitializeComponent();
        _schedule = existingSchedule;

        if (_schedule != null)
        {
            Text = "Edit Schedule";
            dtpTime.Value = DateTime.Today.Add(_schedule.Timing);
            chkActive.Checked = _schedule.IsActive;
        }
        else
        {
            Text = "Add Schedule";
            dtpTime.Value = DateTime.Now.AddMinutes(1);
            chkActive.Checked = true;
        }

        SetupEventHandlers();
    }

    private void SetupEventHandlers()
    {
        btnSave.Click += BtnSave_Click;
        btnCancel.Click += BtnCancel_Click;
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        var timing = dtpTime.Value.TimeOfDay;

        if (_schedule == null)
        {
            _schedule = new TkSchedule
            {
                Timing = timing,
                IsActive = chkActive.Checked
            };
        }
        else
        {
            _schedule.Timing = timing;
            _schedule.IsActive = chkActive.Checked;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
