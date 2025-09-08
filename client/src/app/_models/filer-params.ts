import { Gender } from "./gender";
import { LoggedInUser } from "./logged-in-user";

export class FilterParams {
  gender?: Gender;
  minAge?: number;
  maxAge?: number;

  constructor(gender?: Gender, minAge?: number, maxAge?: number) {
    this.gender = gender;
    this.minAge = minAge;
    this.maxAge = maxAge;
  }

  static FromOppositeGender(user: LoggedInUser) : FilterParams {
    return { gender: user.gender === Gender.Male
      ? Gender.Female
      : user.gender === Gender.Female
        ? Gender.Male
        : undefined } as FilterParams
  }
}
