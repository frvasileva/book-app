export class BookAction {
  public enabled: boolean;
  public type: string;
  public checked: boolean;
  public totalChecks: Int16Array;

  constructor(enabled, type, checked, totalChecks) {
    this.enabled = enabled;
    this.type = type;
    this.checked = checked;
    this.totalChecks = totalChecks;
  }
}
