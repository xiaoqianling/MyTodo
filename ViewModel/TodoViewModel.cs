using Todo.Repository;
using Todo.Model;

namespace Todo.ViewModel
{
    [INotifyPropertyChanged]
    public partial class TodoViewModel : BaseViewModel
    {
        private readonly ITodoRepository _todoRepository;

        public ObservableCollection<TodoModel> TodoModels
        {
            private set; get;
        } = new ObservableCollection<TodoModel>();


        public TodoViewModel(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        [RelayCommand]
        async Task TodoTapped(TodoModel todoModel)
        {
            if (IsBusy) return;
            IsBusy = true;
            await Shell.Current.GoToAsync(nameof(TodoDetailsView), true,
                new Dictionary<string, object>()
                {
                    ["TodoModel"] = todoModel,
                    ["SelectedSprintID"] = todoModel.SprintID,
                });
            IsBusy = false;
        }

        [RelayCommand]
        void IsCompleteTapped(TodoModel todoModel)
        {
            Debug.WriteLine($"TodoModel: {todoModel}");
            Task.Run(async () =>
            {
                //var service = await TodoService.Instance;
                await _todoRepository.SaveItem(todoModel);
            });
        }
        [RelayCommand]
        async Task AddTapped()
        {
            if (IsBusy) return;
            IsBusy = true;
            string todoDetailsUri = $"{nameof(TodoDetailsView)}?{nameof(TodoDetailsViewModel.SelectedSprintID)}=10";
            //await Shell.Current.GoToAsync(nameof(TodoDetailsView), true, 
            //    new Dictionary<string, object>()
            //    {
            //        ["SelectedSprintID"]=10
            //    });
            await Shell.Current.GoToAsync("/details?SelectedSprintID=99&Test=77");
            IsBusy = false;
        }

        [RelayCommand]
        async Task GotoSprintView()
        {
            if (IsBusy) return;
            IsBusy = true;
            await Shell.Current.GoToAsync(nameof(SprintView));
            IsBusy = false;
        }

        [RelayCommand]
        async Task GotoMainPage()
        {
            if (IsBusy) return;
            IsBusy = true;
            await Shell.Current.GoToAsync("..");
            IsBusy = false;
        }

        internal void Refresh()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            Task.Run<List<TodoModel>>(async () =>
            {
                //var service = await TodoService.Instance;
                var tasks = await _todoRepository.GetItems();
                tasks.Sort((t1, t2) =>
                {
                    if (t1.IsComplete == t2.IsComplete)
                    {
                        return t1.Id < t2.Id ? -1 : 1;
                    }
                    else if (t1.IsComplete)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                });
                Debug.WriteLine($"RESULT {tasks.Count}");
                return tasks;
            }).
            ContinueInMainThreadWith((taskTables) =>
            {
                TodoModels.Clear();
                if (taskTables != null && taskTables.Count > 0)
                {
                    taskTables.ForEach((t) => { TodoModels.Add(t); });
                }

                IsBusy = false;
            });

        }
    }
}
