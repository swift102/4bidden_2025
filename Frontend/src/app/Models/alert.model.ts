// alert.model.ts
export class Alert {
  constructor(
    public type: 'success' | 'error' | 'info' | 'warning',
    public message: string
  ) {}
}
