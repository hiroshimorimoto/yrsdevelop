import axios from 'axios'

var BankData = null;
var Banks = new Array();
export default class ResourceUtil {
    constructor() { }

    static _addBank(banks) {
        banks.forEach((bank) => {
            //this.banks.push(bank);
            Banks.push(bank);
        });
    };

    static async GetBankData() {
        //if (this.BankData != null) {
        if (BankData != null) {
            //console.log("GetBankData RETURNED!!!");
            //return this;
            return {
                BankData: BankData,
                Banks: Banks
            }
        }

        //console.log("GetBankData EXECUTED!!!");

        return new Promise((resolve, reject) => {

            //エリアデータJsonの解析
            const url = process.env.VUE_APP_API_BASE_URL + "/resource/bank_data.json";
            axios.get(url)
                .then((res) => {
                    //this.BankData = res.data;
                    BankData = res.data;
                    //this._addBank(this.BankData.banks);
                    this._addBank(BankData.Banks);
                    //resolve(this);
                    resolve({
                        BankData: BankData,
                        Banks: Banks
                    });
                })
                .catch(err => {
                    reject(err);
                })
        })
    };
}