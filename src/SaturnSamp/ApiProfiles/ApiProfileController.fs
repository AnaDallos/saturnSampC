namespace ApiProfiles

open Saturn
open Giraffe.ResponseWriters


module Controller =

    let indexAction =
        text ("Hello world!!")

    let ApiProfileHW = router {
        get "/" indexAction
    }