﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Elsa.Attributes;
using Elsa.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Services.Models
{
    public class ActivityExecutionContext
    {
        public ActivityExecutionContext(
            WorkflowExecutionContext workflowExecutionContext,
            IServiceProvider serviceProvider,
            IActivityBlueprint activityBlueprint,
            object? input = null)
        {
            WorkflowExecutionContext = workflowExecutionContext;
            ServiceProvider = serviceProvider;
            ActivityBlueprint = activityBlueprint;
            Input = input;
            Outcomes = new List<string>(0);
        }

        public WorkflowExecutionContext WorkflowExecutionContext { get; }
        public IServiceProvider ServiceProvider { get; }
        public IActivityBlueprint ActivityBlueprint { get; }
        public object? Input { get; }
        public object? Output { get; set; }
        public IReadOnlyCollection<string> Outcomes { get; set; }

        public void SetVariable(string name, object? value) => WorkflowExecutionContext.SetVariable(name, value);
        public object? GetVariable(string name) => WorkflowExecutionContext.GetVariable(name);
        public T GetVariable<T>(string name) => WorkflowExecutionContext.GetVariable<T>(name);
        public T GetService<T>() => WorkflowExecutionContext.ServiceProvider.GetService<T>();

        public async ValueTask SetActivityPropertiesAsync(
            IActivity activity,
            CancellationToken cancellationToken = default) =>
            await WorkflowExecutionContext.ActivityPropertyProviders.SetActivityPropertiesAsync(
                activity,
                this,
                cancellationToken);

        public IActivity ActivateActivity(Action<IActivity>? setupActivity = default)
        {
            var activityActivator = ServiceProvider.GetRequiredService<IActivityActivator>();
            return activityActivator.ActivateActivity(setupActivity);
        }
    }
}