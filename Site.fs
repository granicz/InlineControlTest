namespace MyApp01

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Server
open WebSharper.UI.Html
open WebSharper.UI.Templating

type EndPoint =
    | [<EndPoint "/">] Home

type MyControl(a: string) =
    inherit Web.Control()

    [<JavaScript>]
    override this.Body =
        div [] [text $"{a} This is a web.control instance"]

type Templates = Template<"wwwroot\\template.html", ClientLoad.Inline, ServerLoad.WhenChanged>

module Site =
    open type WebSharper.UI.ClientServer
    open WebSharper.JavaScript

    [<Website>]
    let Main =
        Application.MultiPage (fun ctx endpoint ->
            match endpoint with
            | EndPoint.Home ->
                let a = "hello"
                // The master document is coming from a template, with ...
                Templates()
                    .Title("Hello")
                    .Body([
                        // 1. Embedded client-side code
                        client (div [] [text $"1. non-hydrated {a}"])
                        // 2. Hydrated client-side code
                        hydrate (div [] [text $"2. hydrated {a}"])
                        // 3. An OAR handler
                        div [
                            on.afterRender (fun e -> Console.Log $"br OAR={e}")
                        ] [text "3."]
                        // 4. A click event handler
                        div [
                            on.click (fun e args -> Console.Log $"div onclick={e}")
                        ] [text "4. Click me"]
                        // 5. A client-side control
                        Doc.WebControl <| MyControl("5.")
                        // 6. A template-driven "component" with event binding
                        Templates.MyComponent()
                            .Title("6. MyButton")
                            .OnClick(fun e -> Console.Log $"MyButton onclick => Textbox has \"{e.Vars.Textbox.Value}\"")
                            .Doc()
                    ])
                    .Doc()
                |> Content.Page
        )
