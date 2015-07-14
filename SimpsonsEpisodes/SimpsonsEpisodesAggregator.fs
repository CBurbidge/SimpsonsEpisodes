module SimpsonsEpisodes.Aggregator

open System
open System.IO
open System.Web
open FSharp.Configuration
open FSharp.Data
open SimpsonsEpisodes.Parser

/// The description comes from the season page description of the episode and the summary and plot come from each individual episode page.
type SimpsonsEpisode(seasonNumber: int, episodeNumber:int, description:string, summary: string, plot: string) =
    member this.seasonNumber = seasonNumber
    member this.episodeNumber = episodeNumber
    member this.description = description
    member this.summary = summary
    member this.plot = plot

type SimpsonsSeason(seasonNumber: int, episodes: SimpsonsEpisode list) =
    member this.seasonNumber = seasonNumber
    member this.episodes = episodes

type SimpsonsFilmography(seasons: SimpsonsSeason list) =
    member this.seasons = seasons

let getFilmography(allEpisodes: Episode list) =
    let simpsonsEpisodes = episodes
                           |> List.map (fun ep -> 
                           ep
                           )