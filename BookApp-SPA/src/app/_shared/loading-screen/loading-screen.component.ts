import { Component, OnDestroy, OnInit } from "@angular/core";
import { Subscription } from "rxjs";
import { LoadingScreenService } from "../../_services/loading-screen.service";
import { debounceTime } from "rxjs/operators";

@Component({
  selector: "app-loading-screen",
  templateUrl: "./loading-screen.component.html",
  styleUrls: ["./loading-screen.component.scss"]
})
export class LoadingScreenComponent implements OnInit, OnDestroy {

  loading: boolean = false;
  loadingSubscription: Subscription;

  constructor(private loadingScreenService: LoadingScreenService) {}

  ngOnInit() {
    this.loadingSubscription = this.loadingScreenService.loadingStatus
      .pipe(debounceTime(200))
      .subscribe((value: boolean) => {
        this.loading = value;
      });
  }

  ngOnDestroy() {
    this.loadingSubscription.unsubscribe();
  }
}
