open SimpsonsEpisodes
open System

[<EntryPoint>]
let main argv = 
    
    SimpsonsEpisodes.downloadSeasonFilesToDisk
    
    let allEpisodes: EpisodeSummaryInfo list = SimpsonsEpisodes.getAllEpisodes()
    
    SimpsonsEpisodes.ensureThatEpisodeFilesExist allEpisodes
    for episode in allEpisodes do
        let episodeModel = SimpsonsEpisodes.parseEpisodeFile(episode, getEpisodeFileName)
        0 |> ignore
    Console.ReadKey() |> ignore
    0 // return an integer exit code