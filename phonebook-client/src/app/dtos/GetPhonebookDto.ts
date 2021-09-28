export class GetPhonebookDto {
  public id: number;
  public email: string;
  public name: string;
  public surname: string;
  public homePhoneNumber: string;
  public cellPhoneNumber: string;
  public district: District;

  constructor() {}
}

export class District {
  public id: number;
  public postCode: string;
  public name: string;
}
