namespace PredictFlow.Application.DTOs.InvitationDtos;

public class InvitationValidateResultDto
{ 
        public bool CodeIsValid { get; set; }
        public bool InvitationIsExpired { get; set; }
        public bool EmailExists { get; set; }
        public bool UserAlreadyInTeam { get; set; }

        
        public string Message { get; set; } = string.Empty;
    
}