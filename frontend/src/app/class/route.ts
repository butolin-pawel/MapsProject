import { Time } from "@angular/common"

export class Route {
  id! : number
  name! : string
  length! : number
  description! : string
  time!: Time
  paid! : boolean
}
