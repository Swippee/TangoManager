namespace TangoManagerAPI.DTO;

public class CreateQuizRequest
{
    public string PacketName { get; set; }
}

public class AnswerQuizRequest
{
    public int QuizId { get; set; }
    public string Answer { get; set; }
}

public class AddCardToPacketRequest
{
    public string Question { get; set; }
    public string Answer { get; set; }
    public decimal Score { get; set; }
}

public class CreatePacketRequest
{
    public string Name { get; set; }
    public string Description { get; set; }

}