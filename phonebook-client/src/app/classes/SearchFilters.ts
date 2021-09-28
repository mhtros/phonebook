export class SearchFilter {
  public filterProperties: Array<FilterProperty>;
  public orderBy: string;
  public orderByDirection: string;
}

export class FilterProperty {
  public propertyName: string;
  public propertyValue: string;
}
