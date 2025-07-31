// import NextAuth, { DefaultSession, DefaultUser } from "next-auth";

// declare module "next-auth" {
//   interface User extends DefaultUser {
//     token: string;
//   }

//   interface Session extends DefaultSession {
//     user: User;
//   }
// }

import NextAuth, { DefaultSession, DefaultUser } from "next-auth";

declare module "next-auth" {
  interface User extends DefaultUser {
    isNeed2Stage: boolean;
    user2StageData?: {
      urlSegmentPart1: string;
      urlSegmentPart2: string;
      validTo: Date;
    };
    token?: string;
    refreshToken?: string;
    jwtValidTo?: Date;
    refreshTokenValidTo?: string;
    isIndividual: boolean;
  }

  interface Session extends DefaultSession {
    user: User;
  }
}

declare module "next-auth/jwt" {
  interface JWT {
    accessToken: string;
    refreshToken?: string;
    jwtValidTo?: Date;
    refreshTokenValidTo?: string;
    isIndividual: boolean;
  }
}
