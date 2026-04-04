using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using ONE_Classics_Nexus.WinForms.Services;
using ONE_Classics_Nexus.Application.Requests;
using ONE_Classics_Nexus.Application.UseCases.Vehicles;
using ONE_Classics_Nexus.Application.UseCases.Appraisals;
using ONE_Classics_Nexus.Application.UseCases.Inspections;

namespace ONE_Classics_Nexus.WinForms.Forms
{
    /// <summary>
    /// Simple WinForms test harness for exercising API endpoints.
    /// Provides buttons to create a vehicle, appraisal, inspection and to save inspection scores.
    /// </summary>
    public sealed class MainForm : Form
    {
        // API client used to call backend endpoints.
        private readonly ApiClient _apiClient;

        // UI controls
        private readonly Button _btnCreateVehicle;
        private readonly Button _btnCreateAppraisal;
        private readonly Button _btnCreateInspection;
        private readonly Button _btnSaveScores;
        private readonly TextBox _txtOutput;

        // IDs returned from the API and used to chain operations.
        private Guid _vehicleId;
        private Guid _appraisalId;
        private Guid _inspectionId;

        /// <summary>
        /// Constructs the main form and wires UI event handlers.
        /// </summary>
        public MainForm(ApiClient apiClient)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));

            Text = "ONE Classics Nexus - Phase 1 Test Harness";
            Width = 900;
            Height = 600;
            StartPosition = FormStartPosition.CenterScreen;

            _btnCreateVehicle = new Button
            {
                Text = "Create Vehicle",
                Left = 20,
                Top = 20,
                Width = 150
            };

            _btnCreateAppraisal = new Button
            {
                Text = "Create Appraisal",
                Left = 190,
                Top = 20,
                Width = 150
            };

            _btnCreateInspection = new Button
            {
                Text = "Create Inspection",
                Left = 360,
                Top = 20,
                Width = 150
            };

            _btnSaveScores = new Button
            {
                Text = "Save Scores",
                Left = 530,
                Top = 20,
                Width = 150
            };

            _txtOutput = new TextBox
            {
                Left = 20,
                Top = 70,
                Width = 820,
                Height = 450,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true
            };

            Controls.Add(_btnCreateVehicle);
            Controls.Add(_btnCreateAppraisal);
            Controls.Add(_btnCreateInspection);
            Controls.Add(_btnSaveScores);
            Controls.Add(_txtOutput);

            // Wire event handlers to async methods. Exceptions are caught inside methods.
            _btnCreateVehicle.Click += async (_, _) => await CreateVehicleAsync();
            _btnCreateAppraisal.Click += async (_, _) => await CreateAppraisalAsync();
            _btnCreateInspection.Click += async (_, _) => await CreateInspectionAsync();
            _btnSaveScores.Click += async (_, _) => await SaveScoresAsync();
        }

        /// <summary>
        /// Calls the API to create a vehicle and writes results to the output box.
        /// </summary>
        private async Task CreateVehicleAsync()
        {
            try
            {
                var request = new CreateVehicleRequest
                {
                    Vin = $"WINFORMS-{DateTime.UtcNow:yyyyMMddHHmmss}",
                    Year = 1970,
                    Make = "Chevrolet",
                    Model = "K10",
                    Trim = "Custom Deluxe",
                    BodyStyle = "Pickup",
                    Engine = "6.0L LS",
                    Transmission = "4L80E",
                    Drivetrain = "4x4",
                    TitleStatus = "Clean"
                };

                var result = await _apiClient.CreateVehicleAsync(request);
                if (result is null)
                {
                    WriteLine("CreateVehicleAsync returned null.");
                    return;
                }

                _vehicleId = result.VehicleId;

                WriteLine("Vehicle created successfully.");
                WriteLine($"VehicleId: {_vehicleId}");
                WriteLine($"VIN: {result.Vin}");
                WriteLine(string.Empty);
            }
            catch (Exception ex)
            {
                WriteLine($"CreateVehicleAsync ERROR: {ex.Message}");
                WriteLine(string.Empty);
            }
        }

        /// <summary>
        /// Calls the API to create an appraisal for the previously created vehicle.
        /// </summary>
        private async Task CreateAppraisalAsync()
        {
            try
            {
                if (_vehicleId == Guid.Empty)
                {
                    WriteLine("Create a vehicle first.");
                    WriteLine(string.Empty);
                    return;
                }

                var request = new CreateAppraisalRequest
                {
                    VehicleId = _vehicleId
                };

                var result = await _apiClient.CreateAppraisalAsync(request);
                if (result is null)
                {
                    WriteLine("CreateAppraisalAsync returned null.");
                    return;
                }

                _appraisalId = result.AppraisalId;

                WriteLine("Appraisal created successfully.");
                WriteLine($"AppraisalId: {_appraisalId}");
                WriteLine($"VehicleId: {result.VehicleId}");
                WriteLine(string.Empty);
            }
            catch (Exception ex)
            {
                WriteLine($"CreateAppraisalAsync ERROR: {ex.Message}");
                WriteLine(string.Empty);
            }
        }

        /// <summary>
        /// Calls the API to create an inspection for the previously created appraisal.
        /// </summary>
        private async Task CreateInspectionAsync()
        {
            try
            {
                if (_appraisalId == Guid.Empty)
                {
                    WriteLine("Create an appraisal first.");
                    WriteLine(string.Empty);
                    return;
                }

                var request = new CreateInspectionRequest
                {
                    AppraisalId = _appraisalId,
                    InspectionType = "Standard"
                };

                var result = await _apiClient.CreateInspectionAsync(request);
                if (result is null)
                {
                    WriteLine("CreateInspectionAsync returned null.");
                    return;
                }

                _inspectionId = result.InspectionId;

                WriteLine("Inspection created successfully.");
                WriteLine($"InspectionId: {_inspectionId}");
                WriteLine($"Seeded score count: {result.ConditionScores.Count}");
                WriteLine(string.Empty);
            }
            catch (Exception ex)
            {
                WriteLine($"CreateInspectionAsync ERROR: {ex.Message}");
                WriteLine(string.Empty);
            }
        }

        /// <summary>
        /// Calls the API to save inspection condition scores.
        /// Note: fixes invalid collection initializer by using a List{T}.
        /// </summary>
        private async Task SaveScoresAsync()
        {
            try
            {
                if (_inspectionId == Guid.Empty)
                {
                    WriteLine("Create an inspection first.");
                    WriteLine(string.Empty);
                    return;
                }

                var request = new SaveInspectionRequest
                {
                    InspectionId = _inspectionId,
                    Scores = new List<SaveConditionScoreRequest>
                    {
                        new SaveConditionScoreRequest
                        {
                            SystemNumber = 1,
                            SystemKey = "exterior_body_paint",
                            Score0To5 = 4.5m,
                            Weight = 1.0m,
                            IsRedFlag = false,
                            Notes = "WinForms test save."
                        },
                        new SaveConditionScoreRequest
                        {
                            SystemNumber = 2,
                            SystemKey = "glass_trim_lighting",
                            Score0To5 = 4.0m,
                            Weight = 0.7m,
                            IsRedFlag = false,
                            Notes = "WinForms test save."
                        }
                    }
                };

                var result = await _apiClient.SaveInspectionScoresAsync(request);
                if (result is null)
                {
                    WriteLine("SaveInspectionScoresAsync returned null.");
                    return;
                }

                WriteLine("Scores saved successfully.");
                WriteLine($"InspectionId: {result.InspectionId}");
                WriteLine($"Returned score count: {result.ConditionScores.Count}");
                WriteLine(string.Empty);
            }
            catch (Exception ex)
            {
                WriteLine($"SaveInspectionScoresAsync ERROR: {ex.Message}");
                WriteLine(string.Empty);
            }
        }

        /// <summary>
        /// Appends a line to the output textbox (thread-affinity note: called from UI event handlers).
        /// </summary>
        private void WriteLine(string message)
        {
            _txtOutput.AppendText(message + Environment.NewLine);
        }
    }
}