namespace Results

open WebSharper
open WebSharper.JavaScript
open WebSharper.Html.Client
open WebSharper.D3

[<JavaScript>]
module Client =

    let Main () =
        let canvas = Canvas[Id "canvas"]
        let d3Canvas = D3.Select("#canvas")
        Div [
            canvas
        ]
