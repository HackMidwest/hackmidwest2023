export interface UnclaimedPostModel {
  time: Date;
  title: string;
  author: string;
  type: string;
  text: string;
  zipCode: number;
  profilePictureUrl: string;
  foodTypePictureUrl: string;
  id: number;
}
