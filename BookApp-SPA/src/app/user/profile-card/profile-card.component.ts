import { Component, OnInit, Input } from "@angular/core";
import { Profile } from 'src/app/_models/profile';

@Component({
  selector: "app-profile-card",
  templateUrl: "./profile-card.component.html",
  styleUrls: ["./profile-card.component.scss"]
})
export class ProfileCardComponent implements OnInit {
  @Input() profile: Profile;
  @Input() isCurrentUser: boolean;
  constructor() {}

  ngOnInit() {}
}
