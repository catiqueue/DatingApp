import { Gender } from "./gender";

export type LoggedInUser = {
  username: string;
  knownAs: string;
  gender: Gender;
  token: string;
  avatarUrl: string | null
}
