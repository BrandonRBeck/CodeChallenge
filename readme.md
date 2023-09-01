# About - Henry Meds Code Challenge

This is the Reservation - Backend V2 code challenge for HenryMeds. 
Please note that this project is generated from a stubbed project I generally use for my own code testing.
The only work I've done on this branch as of the initial commit is updating this readme, and as such, 
the timestamp on the first commit could reasonably be used to show when I started working on this project.
I ended up spending closer to 2 and a half hours on this project.

## How to use
Begin by adding a user of type provider

POST /brandon-com/henry-meds/users
{
  "firstName": "Brandon",
  "lastName": "Beck",
  "userType": "Provider"
}

Add a patient

POST /brandon-com/henry-meds/users
{
  "firstName": "Daniel",
  "lastName": "Harp",
  "userType": "patient"
}

Make note of generated User guids for use adding schedules and appts.

Next request Availability
GET /brandon-com/henry-meds/reservation/availabiltiy?providerId=[providerId you generated]

select an appointment you're interested in
POST /brandon-com/henry-meds/reservations/reserve
{
  "userId": "[the patient guid from the user create patient]",
  "appointmentId": "[the appointment guid from the previous response]"
}

finally, confirm that appointment
POST /brandon-com/henry-meds/reservations/confirm
{
  "userId": "[the patient guid from the user create patient]",
  "appointmentId": "[the appointment guid from the previous response]"
}


## Persistence
I used an in Memory database to handle persistence instead of worrying about standing up a service of some kind that a user would have to replicate.

## Testing
I added tests to handle at the bare minimum the "additional requirements" section. I have very much intended to come back
and flesh out this section, but I found that I ran out of time.

## Tradeoffs
I ended up spending too long on the in memory DB provider, and should have spent more focus on the API design itself. As it stands, its completely functional, but could have certainly been cleaner, better error handling, better user functionality.


## About Brandon

See more about Brandon [here](https://therealbrandon.org)
