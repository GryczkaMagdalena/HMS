export class LoadHandlerService {
  public isRequesting = false;

  constructor() {
  }

  public getStatus(): boolean {
    // console.log('getStatus -> isRequesting: ', this.isRequesting);
    return this.isRequesting;
  }

  public setBusy(): void {
    // console.log('setBusy -> isRequesting: true');
    this.isRequesting = true;
  }

  public setFree(): void {
    // console.log('setBusy -> isRequesting: false');
    this.isRequesting = false;
  }
}
