using MediatorLibrary;

namespace ConsoleClient;

public class CreateUser : ICommand<ResponseState> { public string Name { get; set; } }
public class UpdateUser : ICommand<ResponseState> { public User User { get; set; } }