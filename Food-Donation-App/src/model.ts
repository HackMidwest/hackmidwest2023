export interface PostModel {
  time: Date;
  author: string;
  type: string;
  text: string;
  zipCode: number;
  claimant: number;
}

export interface UnclaimedPostModel {
  time: Date;
  author: string;
  type: string;
  text: string;
  zipCode: number;
  profilePictureUrl: string;
  foodTypePictureUrl: string;
}


export interface LoginModel {
  id: number;
  name: string;
  pictureUrl: string;
}
