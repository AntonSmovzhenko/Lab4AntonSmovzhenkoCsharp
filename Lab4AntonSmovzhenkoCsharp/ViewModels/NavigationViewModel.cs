using Lab4AntonSmovzhenkoCsharp.Navigation;

namespace Lab4AntonSmovzhenkoCsharp.ViewModels
{

    
    internal class NavigationViewModel : BaseNavigationViewModel
    {
        
        public NavigationViewModel()
        {
            Navigate(NavigationTypes.Info);
        }

        protected override NavigationInProject CreateNewViewModel(NavigationTypes type)
        {
            switch (type)
            {
                case NavigationTypes.Login:
                    return new LoginViewModel(() => Navigate(NavigationTypes.Info));
                case NavigationTypes.Info:
                    return new InfoViewModel(() => Navigate(NavigationTypes.Login), person => NavigateToRedactor(person), () => Navigate(NavigationTypes.Info));
                default:
                    return null;
            }
        }

    }
}
