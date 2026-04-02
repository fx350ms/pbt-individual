using Pbt.Individual.Configuration.Dto;
using System.Threading.Tasks;

namespace Pbt.Individual.Configuration;

public interface IConfigurationAppService
{
    Task ChangeUiTheme(ChangeUiThemeInput input);
}
