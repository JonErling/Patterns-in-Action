using System.Collections.ObjectModel;
using System.Windows.Input;
using WpfApp.Models;

namespace WpfApp.ViewModels
{

    // ViewModel for Member.
    // ** MV Patterns: MV-VM Design Pattern.

    public class MemberViewModel : ViewModelBase
    {
        private readonly IProvider _provider;

        public ObservableCollection<MemberModel> Members { get; }

        public CommandModel AddCommandModel { private set; get; }
        public CommandModel EditCommandModel { private set; get; }
        public CommandModel DeleteCommandModel { private set; get; }

        public MemberViewModel(IProvider provider)
        {
            _provider = provider;

            Members = new ObservableCollection<MemberModel>();

            AddCommandModel = new AddCommand(this);
            EditCommandModel = new EditCommand(this);
            DeleteCommandModel = new DeleteCommand(this);
        }


        // indicates whether the member data has been loaded

        public bool IsLoaded { private set; get; }

        // gets a new member.

        public MemberModel NewMemberModel => new MemberModel(_provider);

        // indicates whether a new member can be added

        public bool CanAdd => IsLoaded;


        // indicates whether a member is currently selected

        public bool CanEdit => IsLoaded && CurrentMember != null;


        // indicates whether a member is selected that can be deleted

        public bool CanDelete => IsLoaded && CurrentMember != null;


        // indicates whether a member is selected and orders can be viewed

        public bool CanViewOrders => IsLoaded && CurrentMember != null;


        // retrieves and displays members in given sort order

        public void LoadMembers()
        {
            string sortExpression = "CompanyName ASC";
            foreach (var member in _provider.GetMembers(sortExpression))
                Members.Add(member);

            if (Members.Count > 0)
                CurrentMember = Members[0];

            IsLoaded = true;
        }


        // Clear members from display

        public void UnloadMembers()
        {
            Members.Clear();

            CurrentMember = null;
            IsLoaded = false;
        }

        private MemberModel _currentMemberModel;
        public MemberModel CurrentMember
        {
            get { return _currentMemberModel; }
            set
            {
                if (_currentMemberModel != value)
                {
                    _currentMemberModel = value;
                    OnPropertyChanged("CurrentMember");
                }
            }
        }

        #region Private Command classes

        // implementation of the Add Command

        private class AddCommand : CommandModel
        {
            private readonly MemberViewModel _viewModel;

            public AddCommand(MemberViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            public override void OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                var member = e.Parameter as MemberModel;

                // check that all values have been entered

                e.CanExecute =
                    (!string.IsNullOrEmpty(member.Email)
                  && !string.IsNullOrEmpty(member.CompanyName)
                  && !string.IsNullOrEmpty(member.City)
                  && !string.IsNullOrEmpty(member.Country));

                e.Handled = true;
            }

            public override void OnExecute(object sender, ExecutedRoutedEventArgs e)
            {
                var member = e.Parameter as MemberModel;
                member.Add();

                _viewModel.Members.Add(member);
                _viewModel.CurrentMember = member;
            }
        }


        // implementation of the Edit command

        private class EditCommand : CommandModel
        {
            private MemberViewModel _viewModel;

            public EditCommand(MemberViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            public override void OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                var member = e.Parameter as MemberModel;

                // check that all values have been set

                e.CanExecute = (member.MemberId > 0
                  && !string.IsNullOrEmpty(member.Email)
                  && !string.IsNullOrEmpty(member.CompanyName)
                  && !string.IsNullOrEmpty(member.City)
                  && !string.IsNullOrEmpty(member.Country));

                e.Handled = true;
            }

            public override void OnExecute(object sender, ExecutedRoutedEventArgs e)
            {
                var memberModel = e.Parameter as MemberModel;
                memberModel.Update();
            }
        }


        // implementation of the Delete command

        private class DeleteCommand : CommandModel
        {
            private readonly MemberViewModel _viewModel;

            public DeleteCommand(MemberViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            public override void OnCanExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = _viewModel.CanDelete;
                e.Handled = true;
            }

            public override void OnExecute(object sender, ExecutedRoutedEventArgs e)
            {
                var memberModel = _viewModel.CurrentMember;

                if (memberModel.Delete() > 0)
                {
                    _viewModel.Members.Remove(memberModel);
                    e.Handled = true;
                }
            }
        }

        #endregion
    }
}
