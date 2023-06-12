export class AccessTokenModel {
    constructor(
        public accessToken : string,
        public refreshToken: string
    ) {}
}

export interface CryptoData {
    symbol: string;
    bidPrice: number;
    askPrice: number;
    lastId: number;
}

export interface ChangeSymbolRequest {
    symbol: string;
}