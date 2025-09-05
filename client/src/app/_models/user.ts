import { Gender } from "./gender"
import { Photo } from "./photo"

export type User = {
  id: number
  userName: string
  dateOfBirth: Date
  age: number
  avatarUrl: string
  knownAs: string
  createdAt: Date
  lastActive: Date
  gender: Gender
  introduction: string
  interests: string
  lookingFor: string
  city: string
  country: string
  photos: Photo[]
  roles: string[]
}
