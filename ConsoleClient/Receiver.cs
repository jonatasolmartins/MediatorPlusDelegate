using MediatorLibrary;

namespace ConsoleClient;

public class CreateUserCommandHandler : IReceiver<CreateUser, ResponseState>
{
    public ResponseState  Handle(CreateUser command)
    {
        var user = new User() {Id = 1, Name = "ToUpperCommand"};
        return new ResponseState() {Result = user.Id, StatusCode = 200};
    }
}

public class UpdateUserCommandHandler : IReceiver<UpdateUser, ResponseState>
{
    public ResponseState  Handle(UpdateUser command)
    {
        command.User.Name = command.User.Name.ToUpper();
        return new ResponseState() {Result = "User updated", StatusCode = 200};
    }
}