import { Component, OnInit } from "@angular/core";
import {
  UIParams,
  UIResponse,
  FacebookService,
  InitParams
} from "ngx-facebook";

@Component({
  selector: "app-invite-friend",
  templateUrl: "./invite-friend.component.html",
  styleUrls: ["./invite-friend.component.scss"]
})
export class InviteFriendComponent implements OnInit {
  constructor(private fb: FacebookService) {
    const initParams: InitParams = {
      appId: "237494102958046",
      xfbml: true,
      version: "v2.8"
    };

    fb.init(initParams);
  }

  ngOnInit() {}

  sharePage(url: string) {
    let params: UIParams = {
      href: "https://github.com/zyra/ngx-facebook",
      method: "send",
      link: url
    };

    this.fb
      .ui(params)
      .then((res: UIResponse) => console.log(res))
      .catch((e: any) => console.error(e));
  }
}
