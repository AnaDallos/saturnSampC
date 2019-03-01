namespace Hello

open Giraffe.GiraffeViewEngine
open Saturn

module Views =
    let index=
        div[][
            h2[][rawText "Hello from Saturn"]
        ]
    
    let index2(name: string)=
        div[][
            h2 [] [rawText("Hello " + name + "!")]
        ]
        