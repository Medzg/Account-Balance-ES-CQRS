using AccountBalance_CQRS_ES.Domain.Aggregate;
using AccountBalance_CQRS_ES.Domain.CommandHandlers;
using AccountBalance_CQRS_ES.Domain.Commands;
using AccountBalance_CQRS_ES.Domain.EStore;
using AccountBalance_CQRS_ES.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountBalance_CQRS_ES.ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Test myDomaine = new Test();

            myDomaine.TestMyDomain();

            Console.ReadKey();
        }
    }

    public class Test
    {

        public async void TestMyDomain()
        {
            try { 
            var accountName = "Mohamed Zghal Linedata 123569";
            var accountId = Guid.Parse("1805FB93-2A90-4C9C-B286-EE9A62A94212");
            CreateAccountCommand createAccount = new CreateAccountCommand(accountId, accountName);
            IEventStore eventStore = new EventStore();
            IRepository repository = new Repository(eventStore);
            CommandHandler commandHandler = new CommandHandler(repository);
            WithdrawCashCommand withdrawCash = new WithdrawCashCommand(accountId,0);
            await commandHandler.Handle(withdrawCash);
        
           
                

              var account = await repository.GetById<Account>(accountId);

            System.Console.WriteLine(String.Format("Your Account Name is  : {0} with Debt {1}", account.AccountName,account.Debt));
            }catch(Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

        }
    }
}
