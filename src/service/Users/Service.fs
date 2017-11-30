module Users.Service

open Users.Repository

let getNewUser userName email =
    let emailMaybe = EmailAddress.create email
    match emailMaybe with
    | Some email ->
        let user = create userName email
        Some user
    | None -> 
        None