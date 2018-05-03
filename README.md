# Stack.Widgets
A collection of widgets for Stack

# Usage
Requires Stack v2.1+ (currently in development).

Reference widgets library in your layout like this:
```xml
xmlns:widgets="clr-namespace:LostTech.Stack.Widgets;assembly=LostTech.Stack.Widgets"
```

Insert your widget(s):
```xml
<widgets:__SOME__WIDGET__ ...></widgets:__SOME__WIDGET__>
```

# Widgets
## WebView
Example:
```xml
<widgets:WebView URL="https://detectmybrowser.com/"/>
```

**URL** (bindable, *OneWay*) - get or set the URL to display

Current version uses IE engine.
