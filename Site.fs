namespace MyApp01

open WebSharper
open WebSharper.Sitelets
open WebSharper.UI.Server
open WebSharper.UI.Html

type EndPoint =
    | [<EndPoint "/">] Home

type MyControl() =
    inherit Web.Control()

    [<JavaScript>]
    override this.Body =
        div [] [text "This is a web.control instance"]

module Site =
    open type WebSharper.UI.ClientServer
    open WebSharper.JavaScript

    [<Website>]
    let Main =
        Application.MultiPage (fun ctx endpoint ->
            match endpoint with
            | EndPoint.Home ->
                let a = "hello"
                Content.Page([
                    client (div [] [text a])
                    br [
                        // ERROR: This won't print anything
                        on.afterRender (fun e -> Console.Log $"br OAR={e}")
                    ] []
                    div [
                        // ERROR: uncommenting causes a runtime error
                        //on.click (fun e args -> Console.Log $"div onclick={e}")
                    ] [Doc.WebControl <| MyControl()]
                ])
        )
