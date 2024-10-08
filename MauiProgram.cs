﻿using CommunityToolkit.Maui;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp.Views.Maui.Controls.Hosting;
using Todo.Context;
using Todo.Repository;

namespace Todo
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            }).UseMauiCommunityToolkit()
            .UseSkiaSharp()
            .Services.AddDbContext<TodoContext>();
            // 单例注入
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<TodoViewModel>();
            builder.Services.AddSingleton<SprintViewModel>();
            builder.Services.AddSingleton<MainPageViewModel>();

#if __ANDROID__
            //builder.Services.AddSingleton<IThemeEnvironment, Todo.ThemeEnvironment>();
#endif
            builder.Services.AddSingleton<AppThemeService>();

            builder.Services.AddSingleton<TodoView>();
            builder.Services.AddSingleton<SprintView>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<ITodoRepository, TodoRepository>();
            builder.Services.AddSingleton<ISprintRepository, SprintRepository>();
            // 多例注入
            builder.Services.AddTransient<TodoDetailsViewModel>();
            builder.Services.AddTransient<SprintDetailsViewModel>();
            builder.Services.AddTransient<TodoDetailsView>();
            builder.Services.AddTransient<SprintDetailsView>();

            Routing.RegisterRoute("todo/details", typeof(TodoDetailsView));
            return builder.Build();
        }
    }
}