// import NextAuth, { DefaultSession, DefaultUser } from "next-auth";

// declare module "next-auth" {
//   interface User extends DefaultUser {
//     token: string;
//   }

//   interface Session extends DefaultSession {
//     user: User;
//   }
// }

import { SigningOptions } from "crypto";
import NextAuth, { DefaultSession, DefaultUser } from "next-auth";

declare module "next-auth" {
  interface User extends DefaultUser {
    isNeed2Stage: boolean;
    user2StageData: {
      urlSegmentPart1: string;
      urlSegmentPart2: string;
      validTo: Date;
    } | null;
    token: string | null;
    refreshToken: string | null;
    jwtValidTo: string | null;
    refreshTokenValidTo: string | null;
    isIndividual: boolean | null;
  }

  interface Session extends DefaultSession {
    user: User;
  }
}

declare module "next-auth/jwt" {
  interface JWT {
    accessToken: string;
    refreshToken: string | null;
    jwtValidTo: string | null;
    refreshTokenValidTo: string | null;
    isIndividual: boolean;
  }
}
