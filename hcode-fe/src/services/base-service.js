import axiosRequest from "@/api/axios-request.js";

class BaseService {
    constructor(baseUrl) {
        this.baseUrl = baseUrl;
        this.baseRequest = axiosRequest;
    }
    /**
     * Get all data
     *
     * Author: nlnhat (03/07/2023)
     * @return Response
     */
    async getAll() {
        const response = await this.baseRequest.get(this.baseUrl);
        return response;
    }
    /**
     * Get 1 data by id
     *
     * Author: nlnhat (03/07/2023)
     * @return Response
     */
    async get(id) {
        const response = await this.baseRequest.get(`${this.baseUrl}/${id}`);
        return response;
    }
    /**
     * Create data
     *
     * Author: nlnhat (03/07/2023)
     * @param {object} data Object to add
     * @return Response
     */
    async post(data) {
        const response = await this.baseRequest.post(this.baseUrl, data);
        return response;
    }
    /**
     * Update data
     *
     * Author: nlnhat (03/07/2023)
     * @param {string | number} id Id of object
     * @param {object} data Object to update
     * @return Response
     */
    async put(id, data) {
        const response = await this.baseRequest.put(
            `${this.baseUrl}/${id}`,
            data
        );
        return response;
    }
    /**
     * Delete 1 record by id
     *
     * Author: nlnhat (03/07/2023)
     * @param {string | number} id Id of object
     * @return Response
     */
    async delete(id) {
        const response = await this.baseRequest.delete(`${this.baseUrl}/${id}`);
        return response;
    }
    /**
     * Delete many records by ids
     *
     * Author: nlnhat (03/07/2023)
     * @param {object} ids List ids to delete
     * @return Response
     */
    async deleteMany(ids) {
        const response = await this.baseRequest.delete(this.baseUrl, {
            data: ids,
        });
        return response;
    }
}

export default BaseService;
