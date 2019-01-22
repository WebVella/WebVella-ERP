<!--{"sort_order":2, "name": "create-your-own", "label": "Create your own"}-->
# Create a Plugin for the WebVella Erp

To create a Plugin you need to add to the solution a Razor Class Library that has specific structure and requirements.

## Plugin name

The naming convention that we follow when creating a plugin is: WebVella.Erp.Plugins.PluginName. You can also add a prefix before the plugin name if needed.

## Folder Structure

The plugin usually has a main `.cs` file and a number of folders that hold the code for the various plugin components. Here is the folder structure we consider best:

<i class="fa fa-fw fa-folder go-orange"></i> Components <br/>
<i class="fa fa-fw fa-folder go-orange"></i> Controllers <br/>
<i class="fa fa-fw fa-folder go-orange"></i> DataSource <br/>
<i class="fa fa-fw fa-folder go-orange"></i> Hooks <br/>
<i class="fa fa-fw fa-folder go-orange"></i> Jobs <br/>
<i class="fa fa-fw fa-folder go-orange"></i> Model <br/>
<i class="fa fa-fw fa-folder go-orange"></i> Pages <br/>
<i class="fa fa-fw fa-folder go-orange"></i> Services <br/>
<i class="fa fa-fw fa-folder go-orange"></i> Utils <br/>
<i class="fa fa-fw fa-file-code go-blue"></i> PluginNamePlugin.cs

## PluginNamePlugin.cs

You can create this file as an ordinary class, but there are several requirements in order to turn it into a plugin:

#### Requirement 1: The Namespace should correspond to the plugin library name
```csharp
namespace WebVella.Erp.Plugins.SDK
```

#### Requirement 2: Should inherit `ErpPlugin`

```csharp
public partial class SdkPlugin : ErpPlugin
```

#### Requirement 3: Should override at least the `Name` property of `ErpPlugin`

```csharp
[JsonProperty(PropertyName = "name")]
public override string Name { get; protected set; } = "sdk";
```

#### Requirement 4: Should implement the `Initialize` method of `ErpPlugin`

Will need to inject the `IServiceProvider`.

```csharp
public override void Initialize(IServiceProvider serviceProvider)
```

## Components

Here are the page components provided by the plugin.

## Controllers

Here are the api controllers provided by the plugin.

## DataSource

Here are the code datasources provided by the plugins

## Hooks

Here are the API hooks provided by the plugins

## Jobs

Here are the background jobs of the plugin

## Model

Plugin's model classes

## Pages

Plugin pages. All plugins can override the Site page routes securely. If you need to override another plugin page route the result is not always constant so we do not advise it.

## Services

Plugin's service methods

## Utils

Plugin's utility methods