export class PaginationParameters {
  public pageNumber: number;
  public pageSize: number;
}

export interface Pagination {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
}

export class Pagination implements Pagination {
  constructor() {}
}

export class PaginatedResult<T> {
  result: T;
  pagination: Pagination;
}
