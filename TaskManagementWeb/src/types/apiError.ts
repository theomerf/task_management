export interface ValidationErrors {
  [field: string]: string[];
}

export interface ApiErrorResponse {
  message?: string;
  errors?: ValidationErrors;
  title?: string;
  status?: number;
}

export interface ApiErrorArrayItem {
  code: string;
  description: string;
}

export type FormError = string | ValidationErrors | ApiErrorResponse;