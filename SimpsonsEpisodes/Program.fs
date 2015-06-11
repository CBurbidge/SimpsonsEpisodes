open System
open FSharp.Data

//let simpsonsEpisodes = new HtmlProvider<"http://en.wikipedia.org/wiki/List_of_The_Simpsons_episodes">()

let episodesUrl = "http://en.wikipedia.org/wiki/List_of_The_Simpsons_episodes"


[<EntryPoint>]
let main argv = 
    let a  = 0
    let episodes = HtmlDocument.Load(episodesUrl)
    let trElements = episodes.Descendants["tr"]
    let episodeRows: seq<HtmlNode> = 
        trElements
        |> Seq.filter(fun (e: HtmlNode) -> 
            e.HasAttribute("class", "vevent")
        )

    Console.ReadKey() |> ignore
    0 // return an integer exit code
