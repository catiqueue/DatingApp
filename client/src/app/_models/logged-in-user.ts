import { Gender } from "./gender";

export type LoggedInUser = {
  userName: string;
  knownAs: string;
  gender: Gender;
  token: string;
  avatarUrl: string | null
}
