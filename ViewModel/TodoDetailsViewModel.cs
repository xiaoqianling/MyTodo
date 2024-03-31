using Todo.Repository;
using Todo.Model;
using Todo.Services;

namespace Todo.ViewModel
{
    [INotifyPropertyChanged]
    [QueryProperty(nameof(Test), nameof(Test))]

    public partial class TodoDetailsViewModel : BaseViewModel, IQueryAttributable
    {
        public AppThemeService _appTheme;

        private readonly ITodoRepository _todoRepository;
        private readonly ISprintRepository _sprintRepository;

        [ObservableProperty]
        private TodoModel _todoModel;

        [ObservableProperty]
        private SprintModel _selectedSprint;

        [ObservableProperty]
        private int _test = 7;

        public int SelectedSprintID
        {
            get; set;
        }


        public ObservableCollection<SprintModel> SprintModels
        {
            private set; get;
        } = new ObservableCollection<SprintModel>();


        public TodoDetailsViewModel(AppThemeService appTheme, ITodoRepository _todoRepository, ISprintRepository sprintRepository)
        {
            _appTheme = appTheme;
            // TodoModel是_todoModel字段自动生成的被观察属性
            TodoModel = new TodoModel();
            this._todoRepository = _todoRepository;
            _sprintRepository = sprintRepository;
        }

        [RelayCommand]
        void Save()
        {
            var temp = SelectedSprint;
            if (TodoModel.Name == null && TodoModel.Description == null)
            {
                Cancel();
            }
            else
            {
                var t = Task.Run(async () =>
                {
                    //var service = await TodoService.Instance;
                    TodoModel.Color = RandomGenerator();
                    TodoModel.SprintID = SelectedSprint.Id;
                    await _todoRepository.SaveItem(TodoModel);
                }).
                    ContinueInMainThreadWith(async () =>
                    {
                        await Shell.Current.GoToAsync("..");
                    });
            }
        }
        [RelayCommand]
        void Delete()
        {
            Task.Run(async () =>
            {
                //var service = await TodoService.Instance;
                await _todoRepository.DeleteItem(TodoModel.Id);

            }).ContinueInMainThreadWith(async () =>
            {
                await Shell.Current.GoToAsync("..");
            });
        }

        [RelayCommand]
        async void Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
        internal void Refresh()
        {
            if (IsBusy)
            {
                return;
            }
            IsBusy = true;

            Task.Run<List<SprintModel>>(async () =>
            {
                List<SprintModel> tasks = await _sprintRepository.GetItems();
                //if no sprint exist create a new sprint with default duration as 7
                if (tasks.Count() == 0)
                {
                    var task = new SprintModel()
                    {
                        SprintDuration = 7
                    };
                    await _sprintRepository.SaveItem(task);
                    tasks.Add(task);
                }
                return tasks;
            }).
            ContinueInMainThreadWith((sprintTables) =>
            {
                if (sprintTables != null && sprintTables.Count > 0)
                {
                    sprintTables.ForEach((t) =>
                    {
                        SprintModels.Add(t);
                    });
                    SelectedSprint = SprintModels.FirstOrDefault(s => s.Id.Equals(SelectedSprintID));

                }

                IsBusy = false;
            });
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            Debug.WriteLine($"查询参数：");
            Debug.WriteLine(query["SelectedSprintID"]);
            // TODO 获取参数
            SelectedSprintID = int.Parse((string)query["SelectedSprintID"]);
            TodoModel.Name = (string)query["SelectedSprintID"];
            //Test = int.Parse((String)query["SelectedSprintID"]);
        }
    }
}
