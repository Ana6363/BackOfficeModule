using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/patient-medical-record")]
public class PatientMedicalRecordController : ControllerBase
{
    private readonly PatientMedicalRecordService _patientMedicalRecordService;
    private readonly ILogger<PatientMedicalRecordController> _logger;

    public PatientMedicalRecordController(PatientMedicalRecordService patientMedicalRecordService, ILogger<PatientMedicalRecordController> logger)
    {
        _patientMedicalRecordService = patientMedicalRecordService;
        _logger = logger;
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePatientMedicalRecord([FromBody] PatientMedicalRecordDto patientMedicalRecord)
    {
        if (patientMedicalRecord == null || string.IsNullOrEmpty(patientMedicalRecord.RecordNumber) || string.IsNullOrEmpty(patientMedicalRecord.Allergies) || string.IsNullOrEmpty(patientMedicalRecord.MedicalConditions) ||string.IsNullOrEmpty(patientMedicalRecord.FullName))
        {
            _logger.LogWarning("Invalid patient medical record data received.");
            return BadRequest("Invalid patient medical record data.");
        }
        try
        {
            var result = await _patientMedicalRecordService.UpdatePatientMedicalRecordAsync(patientMedicalRecord);
            if (result)
            {
                return Created(string.Empty, new { message = "Patient medical record updated successfully." });
            }
            else
            {
                return StatusCode(500, new { message = "Failed to update patient medical record." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the patient medical record.");
            return StatusCode(500, new { message = "An error occurred while updating the patient medical record." });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPatientMedicalRecords()
    {
        try
        {
            var patientMedicalRecords = await _patientMedicalRecordService.GetAllPatientMedicalRecordsAsync();
            if (patientMedicalRecords == null || !patientMedicalRecords.Any())
            {
                return NotFound(new { message = "No patient medical records found." });
            }
            return Ok(new { message = "All patient medical records fetched successfully.", data = patientMedicalRecords });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching patient medical records.");
            return StatusCode(500, new { message = "An error occurred while fetching patient medical records." });
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePatientMedicalRecord(string recorNumber)
    {
        if (string.IsNullOrEmpty(recorNumber))
        {
            _logger.LogWarning("Invalid patient medical record data received.");
            return BadRequest("Invalid patient medical record data.");
        }
        

        try
        {
            var result = await _patientMedicalRecordService.DeletePatientMedicalRecordAsync(recorNumber);
            if (result)
            {
                return Ok( new { message = "Patient medical record deleted successfully." });
            }
            else
            {
                return StatusCode(500, new { message = "Failed to delete patient medical record." });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting the patient medical record.");
            return StatusCode(500, new { message = "An error occurred while deleting the patient medical record." });
        }
    }

    [HttpGet("{recordNumber}/download")]
    public async Task<IActionResult> DownloadPatientMedicalRecord(string recordNumber)
    {
        try
        {
            // Chama o serviço para obter os bytes do arquivo JSON
            var fileBytes = await _patientMedicalRecordService.GetPatientMedicalRecordForDownloadAsync(recordNumber);

            // Retorna o arquivo JSON para o download
            return File(fileBytes, "application/json", $"{recordNumber}_patient_medical_record.json");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching the patient medical record.");
            return StatusCode(500, new { message = "An error occurred while fetching the patient medical record." });
        }
    }
}