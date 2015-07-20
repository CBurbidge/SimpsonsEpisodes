module SimpsonsEpisodes.Plotter

open SimpsonsEpisodes.Analyser
open RDotNet
open RProvider
open RProvider.graphics



let plot (filmography: SimpsonsFilmography) =
    //R.plot()
    
    let widgets = [ 3; 8; 12; 15; 19; 18; 18; 20; ]
    let sprockets = [ 5; 4; 6; 7; 12; 9; 5; 6; ]

    R.plot(widgets)

    R.plot(widgets, sprockets)

    R.barplot(widgets)

    R.hist(sprockets)

    R.pie(widgets)

    0